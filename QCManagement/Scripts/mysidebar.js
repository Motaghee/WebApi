// ----------side bar-------------
$(function () {
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("toggled");
        });

        $(window).resize(function (e) {
            if ($(window).width() <= 768) {
        $("#wrapper").removeClass("toggled");
            } else {
        $("#wrapper").addClass("toggled");
            }
        });
    });


// drop down
(function ($) {
    var CheckboxDropdown = function (el) {
        var _this = this;
        this.isOpen = false;
        this.areAllChecked = false;
        this.$el = $(el);
        this.$label = this.$el.find('.dropdown-label');
        this.$checkAll = this.$el.find('[data-toggle="check-all"]').first();
        this.$inputs = this.$el.find('[type="checkbox"]');

        this.onCheckBox();

        this.$label.on('click', function (e) {
            e.preventDefault();
            _this.toggleOpen();
        });

        this.$checkAll.on('click', function (e) {
            e.preventDefault();
            _this.onCheckAll();
        });

        this.$inputs.on('change', function (e) {
            _this.onCheckBox();
        });
    };

    CheckboxDropdown.prototype.onCheckBox = function () {
        this.updateStatus();
    };

    CheckboxDropdown.prototype.updateStatus = function () {
        var checked = this.$el.find(':checked');

        this.areAllChecked = false;
        this.$checkAll.html('انتخاب همه');

        if (checked.length <= 0) {
            this.$label.html('بدون دخالت در گزارش');
        }
        else if (checked.length === 1) {
            this.$label.html(checked.parent('label').text());
        }
        else if (checked.length === this.$inputs.length) {
            this.$label.html('همه');
            this.areAllChecked = true;
            this.$checkAll.html('حذف همه');
        }
        else {

            //this.$label.html(checked.parent('label').text());
            var SelectedItems = "";
            for (var i = 0; i < this.$inputs.length; i++) {
                if (this.$inputs[i].checked) {
                    if (SelectedItems !="")
                        SelectedItems += ", ";
                    SelectedItems = SelectedItems + this.$inputs[i].value || this.$label.html(checked.parent('label').text());
                    ;
                }
            }
            this.$label.html(SelectedItems);
        }
    };

    CheckboxDropdown.prototype.onCheckAll = function (checkAll) {
        if (!this.areAllChecked || checkAll) {
            this.areAllChecked = true;
            this.$checkAll.html('Uncheck All');
            this.$inputs.prop('checked', true);
        }
        else {
            this.areAllChecked = false;
            this.$checkAll.html('Check All');
            this.$inputs.prop('checked', false);
        }

        this.updateStatus();
    };

    CheckboxDropdown.prototype.toggleOpen = function (forceOpen) {
        var _this = this;

        if (!this.isOpen || forceOpen) {
            this.isOpen = true;
            this.$el.addClass('on');
            $(document).on('click', function (e) {
                if (!$(e.target).closest('[data-control]').length) {
                    _this.toggleOpen();
                }
            });
        }
        else {
            this.isOpen = false;
            this.$el.removeClass('on');
            $(document).off('click');
        }
    };

    var checkboxesDropdowns = document.querySelectorAll('[data-control="checkbox-dropdown"]');
    for (var i = 0, length = checkboxesDropdowns.length; i < length; i++) {
        new CheckboxDropdown(checkboxesDropdowns[i]);
    }
})(jQuery);
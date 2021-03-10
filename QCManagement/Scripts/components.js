function PopupCheckBoxLists() {
    var myTable = document.getElementById("myTable");
    if (!myTable) return;

    var checkBoxLists = myTable.getElementsByTagName("table");

    for (var i = 0; i < checkBoxLists.length; i++) {
        var checkList = checkBoxLists[i];

        var parent = checkList.parentNode;
        var txtCheckedItems = document.createElement('INPUT');
        txtCheckedItems.id = 'txtCheckedItems';
        txtCheckedItems.readOnly = true;
        parent.appendChild(txtCheckedItems);

        var button = document.createElement('INPUT');
        button.type = 'button';
        button.value = '...';
        button.onclick = btnPopupClick;
        parent.appendChild(button);

        //CheckBoxes
        var checkBoxes = checkList.getElementsByTagName('INPUT');
        for (var c = 0; c < checkBoxes.length; c++) {
            checkBoxes[c].onclick = checkBoxClick;
        }

        var divPopup = document.createElement('DIV');

        //divPopupHeader
        var divPopupHeader = document.createElement('DIV');
        divPopupHeader.id = "divPopupHeader";

        var chkSelectAll = document.createElement('input');
        chkSelectAll.type = 'checkbox';
        chkSelectAll.id = 'checkbox' + i;
        divPopupHeader.appendChild(chkSelectAll);
        chkSelectAll.onclick = chkSelectAllChange;
        var lblSelectAll = document.createElement('label');
        lblSelectAll.setAttribute('for', chkSelectAll.id);
        lblSelectAll.innerHTML = '«‰ Œ«» Â„Â';
        divPopupHeader.appendChild(lblSelectAll);

        divPopupHeader.style.display = 'block';
        divPopup.appendChild(divPopupHeader);

        //divPopupBody
        var divPopupBody = document.createElement('DIV');
        divPopupBody.id = "divPopupBody";
        var height = checkBoxes.length * 25;
        var maxHeight = 180;
        divPopupBody.style.height = (height > maxHeight ? maxHeight : height) + 'px';
        divPopupBody.appendChild(checkList);

        divPopup.appendChild(divPopupBody);
        divPopup.style.display = 'none';
        divPopup.id = 'divPopup';
        parent.appendChild(divPopup);

        //divPopupFooter
        var divPopupFooter = document.createElement('DIV');
        divPopupFooter.id = "divPopupFooter";

        var btnClose = document.createElement('input');
        btnClose.type = 'button';
        btnClose.value = '»” ‰';
        btnClose.onclick = btnCloseClick;
        divPopupFooter.appendChild(btnClose);

        divPopup.appendChild(divPopupFooter);

        refreshCheckedItems(divPopup);

        checkList.style.display = 'block';
    }
};

var checkBoxClick = function () {
    var divPopup = this.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
    refreshCheckedItems(divPopup);
};

var refreshCheckedItems = function (divPopup) {
    var txtCheckedItems = divPopup.parentNode.getElementsByTagName('INPUT')[0];
    var chkSelectAll = divPopup.getElementsByTagName('DIV')[0].getElementsByTagName('INPUT')[0];
    var divPopupBody = divPopup.getElementsByTagName('DIV')[1];
    var checkBoxes = divPopupBody.getElementsByTagName('input');
    var checkLabels = divPopupBody.getElementsByTagName('label');

    txtCheckedItems.value = '';
    var isAll = true;
    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            txtCheckedItems.value += (txtCheckedItems.value.length > 0 ? ', ' : '') + checkLabels[i].innerHTML;
        } else {
            isAll = false;
        }
    }
    chkSelectAll.checked = isAll;
};

var btnPopupClick = function () {
    var divPopup = this.parentNode.getElementsByTagName('DIV')[0];
    if (divPopup.style.display != 'block') {
        var myTable = document.getElementById("myTable");
        var divs = myTable.getElementsByTagName("div");
        for (var i = 0; i < divs.length; i++) {
            if (divs[i].id == 'divPopup') {
                divs[i].style.display = 'none';
            }
        }

        divPopup.style.display = 'block';
    } else {
        divPopup.style.display = 'none';
    }
};

var chkSelectAllChange = function () {
    var checked = this.checked;
    var divPopup = this.parentNode.parentNode;
    var divPopupBody = divPopup.getElementsByTagName('DIV')[1];
    var checkBoxes = divPopupBody.getElementsByTagName('input');
    for (var i = 0; i < checkBoxes.length; i++) {
        checkBoxes[i].checked = checked;
    }
    refreshCheckedItems(divPopup);
};

var btnCloseClick = function () {
    var divPopup = this.parentNode.parentNode;
    divPopup.style.display = 'none';
};

namespace wsCacheManager
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ServiceProcess.ServiceInstaller Installer;
            this.WinSrvCacheManagerInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            Installer = new System.ServiceProcess.ServiceInstaller();
            // 
            // Installer
            // 
            Installer.DelayedAutoStart = true;
            Installer.Description = "saipa mobApp CacheManager";
            Installer.DisplayName = "A WS CacheManager";
            Installer.ServiceName = "A WS CacheManager";
            Installer.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            Installer.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.Installer_AfterInstall);
            // 
            // WinSrvCacheManagerInstaller
            // 
            this.WinSrvCacheManagerInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.WinSrvCacheManagerInstaller.Password = null;
            this.WinSrvCacheManagerInstaller.Username = null;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WinSrvCacheManagerInstaller,
            Installer});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller WinSrvCacheManagerInstaller;
    }
}
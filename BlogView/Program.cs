using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlogBusinessLogic.BusinessLogic;
using BlogBusinessLogic.Interfaces;
using BlogDatabaseImplementation.Implementations;
using Unity;
using Unity.Lifetime;

namespace BlogView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IUnityContainer container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormComments>());
        }
        public static IUnityContainer BuildUnityContainer()
        {
            var curContainer = new UnityContainer();
            curContainer.RegisterType<IBlogLogic, BlogLogic>(
                new HierarchicalLifetimeManager());
            curContainer.RegisterType<ICommentLogic, CommentLogic>(
                new HierarchicalLifetimeManager());
            curContainer.RegisterType<ReportLogic>(
                new HierarchicalLifetimeManager());
            curContainer.RegisterType<BackupLogic>(
                new HierarchicalLifetimeManager());
            return curContainer;
        }
    }
}

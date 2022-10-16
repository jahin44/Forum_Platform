using Autofac;
using Forum.System.Contexts;
using Forum.System.Repositories;
using Forum.System.Services;
using Forum.System.UnitOfWorks;

namespace Forum.System
{
    public class SystemModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public SystemModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<SystemDbContext>().As<ISystemDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<BoardRepository>().As<IBoardRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SystemUnitOfWork>().As<ISystemUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<TopicRepository>().As<ITopicRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PostRepository>().As<IPostRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>().InstancePerLifetimeScope();

            builder.RegisterType<TopicService>().As<ITopicService>().InstancePerLifetimeScope();
            builder.RegisterType<BoardService>().As<IBoardService>().InstancePerLifetimeScope();
            builder.RegisterType<PostService>().As<IPostService>().InstancePerLifetimeScope();
            builder.RegisterType<CommentService>().As<ICommentService>().InstancePerLifetimeScope();
            builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
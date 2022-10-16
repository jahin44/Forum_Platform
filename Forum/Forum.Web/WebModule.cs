using Autofac;
using Forum.Web.Models.Account;
using Forum.Web.Models.Comment;
using Forum.Web.Models.Home;
using Forum.Web.Models.Moderator;
using Forum.Web.Models.Post;
using Forum.Web.Models.Topic;

namespace Forum.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ConfirmEmailModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CreateBoardModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<BoardListModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserPostsModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DeleteCommentModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EditCommentModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CreateCommentModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AllCommentsModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DeletePostModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EditPostModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AllPostsModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CreatePostModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DeleteTopicModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EditTopicModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CreateTopicModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AllTopicsModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DeleteBoardModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EditBoardModel>().AsSelf().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}

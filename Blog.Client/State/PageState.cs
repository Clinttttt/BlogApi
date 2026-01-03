namespace Blog.Client.State
{
    public class PageState
    {
        public event Action? Onchange;

        public PostState PostView { get; set; } = PostState.all_post;
        public enum PostState
        {
            all_post,
            published,
            pending,
            drafts,
            bookmarks
        }
        public void View(PostState view)
        {
            PostView = view;
            Onchange?.Invoke();
        }
    }
}

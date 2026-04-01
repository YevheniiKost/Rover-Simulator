namespace RoverSimulator.Presentation.UI
{
    public interface IPresenter<T>
    {
        void AttachView(T view);
        void DetachView();
    }
}
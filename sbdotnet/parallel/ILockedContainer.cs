namespace sbdotnet.parallel
{
    public interface ILockedContainer
    {
        public void Lock();
        public void Unlock();
    }
}

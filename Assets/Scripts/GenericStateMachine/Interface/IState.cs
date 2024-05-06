public interface IState<T>
{
	public void OnStateEnter(T StateObject);
	public void OnStateExit(T StateObject);

}

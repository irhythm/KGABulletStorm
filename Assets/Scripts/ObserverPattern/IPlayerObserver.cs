
public interface IPlayerObserver
{
    public void OnPlayerHpChanged(float curHp, float maxHp);

    public void OnPlayerStateChange();

}

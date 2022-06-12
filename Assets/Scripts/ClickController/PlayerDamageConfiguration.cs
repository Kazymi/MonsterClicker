using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Shop/Create PlayerDamageConfiguration",
    fileName = "PlayerDamageConfiguration", order = 0)]
public class PlayerDamageConfiguration : ScriptableObject
{
    [SerializeField] private int addClickDamageAfterLvlUp;
    [SerializeField] private int addAutoClickDamageAfterLvlUp;
    [SerializeField] private int addSplashAfterLvlUp;

    public int AddClickDamageAfterLvlUp => addClickDamageAfterLvlUp;

    public int AddAutoClickDamageAfterLvlUp => addAutoClickDamageAfterLvlUp;

    public int AddSplashAfterLvlUp => addSplashAfterLvlUp;
}
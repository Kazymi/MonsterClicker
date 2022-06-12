using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Shop/Create ShopConfiguration", fileName = "ShopConfiguration", order = 0)]
public class ShopConfiguration : ScriptableObject
{
    [SerializeField] private int clickPrice;
    [SerializeField] private float addLvlPercentClick;
    [SerializeField] private int autoClickPrice;
    [SerializeField] private float addLvlPercentAutoClick;
    [SerializeField] private int splashPrice;
    [SerializeField] private float addLvlPercentSplash;

    public float AddLvlPercentClick => addLvlPercentClick;

    public float AddLvlPercentAutoClick => addLvlPercentAutoClick;

    public float AddLvlPercentSplash => addLvlPercentSplash;

    public int ClickPrice => clickPrice;

    public int AutoClickPrice => autoClickPrice;

    public int SplashPrice => splashPrice;
}
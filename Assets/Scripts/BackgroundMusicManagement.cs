using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // Kiểm tra nếu đã có nhạc nền tồn tại, hủy bỏ GameObject mới
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            // Đặt nhạc nền này làm duy nhất và không phá hủy khi chuyển scene
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
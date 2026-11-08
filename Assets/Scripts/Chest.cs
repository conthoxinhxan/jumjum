using UnityEngine;

public class Chest : MonoBehaviour 
{
    private bool _isPlayerNearby = false;
    private bool _isOpened = false;

    private Animator _chestAnimator;

    private void Start()
    {
        _chestAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nếu người chơi đang ở gần VÀ bấm phím E VÀ rương chưa mở
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !_isOpened)
        {
            OpenChest();
        }
    }

    // Hàm xử lý khi mở rương
    void OpenChest()
    {
        _isOpened = true;

        // Nếu có hiệu ứng, kích hoạt Trigger trong Animator
        if (_chestAnimator != null)
        {
            _chestAnimator.SetTrigger("Open"); 
        }

        // TODO: Hiện UI thông báo hoặc cộng tiền/vật phẩm cho người chơi tại đây
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _isPlayerNearby = true;
            Debug.Log("Bấm E để mở rương");
            //TODO: bật UI hiển thị chữ "Bấm E để mở" lên màn hình
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _isPlayerNearby = false;
        }
    }
}
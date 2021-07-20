using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitpointsCanvasModelView : MonoBehaviour
{

    [SerializeField] private Text hpText;
    [SerializeField] private Image greenBar;

    [SerializeField] private Camera cam;

    public Text HPAmount {get => hpText; set => hpText = value;}
    public float GreenBarFill { get => greenBar.fillAmount; set => greenBar.fillAmount = value; }

    private void Start()
    {
        cam = CinemachineModelView.Instance.CineCamera.OutputCamera;

        hpText.text = "100%";
        greenBar.fillAmount = 1;

        StartCoroutine(ShowHideHp(5f));
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
    }

    public IEnumerator ShowHideHp(float delay)
    {
        gameObject.TryGetComponent<Canvas>(out Canvas canvas);

        canvas.enabled = true;
        yield return new WaitForSeconds(delay);
        canvas.enabled = false;
    }

    public void ShowHp()
    {
        gameObject.TryGetComponent<Canvas>(out Canvas canvas);
        canvas.enabled = true;
    }
}

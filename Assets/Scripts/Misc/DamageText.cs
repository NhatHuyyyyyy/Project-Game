using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fadeTime = 1f;

    private TextMeshPro textMesh;
    private Color startColor;
    private float timeElapsed = 0f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startColor = textMesh.color;
    }

    public void Setup(int damageAmount)
    {
        textMesh.text = damageAmount.ToString();
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        //Fade out
        timeElapsed += Time.deltaTime;
        if (timeElapsed < fadeTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeTime);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}

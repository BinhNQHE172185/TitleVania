using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenWall : MonoBehaviour
{
    private Player player;
    private Tilemap tilemap;
    [SerializeField] private float fadeRange = 1.0f; // Adjustable fade range
    [SerializeField] private float fadeSpeed = 1.0f; // Speed of fading
    private bool isFading = false;
    private bool isHidden = false; // Tracks if the tilemap is currently hidden

    private void Start()
    {
        player = FindObjectOfType<Player>();
        tilemap = GetComponent<Tilemap>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && !isHidden)
        {
            Debug.Log("Fading Out");
            StartCoroutine(FadeOut());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && isHidden)
        {
            Debug.Log("Fading In");
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float targetAlpha = 0.0f; // Target alpha for fade out
        while (isFading && GetTilemapAlpha() > targetAlpha)
        {
            SetTilemapAlpha(Mathf.MoveTowards(GetTilemapAlpha(), targetAlpha, fadeSpeed * Time.deltaTime));
            yield return null;
        }
        isFading = false;
        isHidden = true;
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        float targetAlpha = 1.0f; // Target alpha for fade in
        while (isFading && GetTilemapAlpha() < targetAlpha)
        {
            SetTilemapAlpha(Mathf.MoveTowards(GetTilemapAlpha(), targetAlpha, fadeSpeed * Time.deltaTime));
            yield return null;
        }
        isFading = false;
        isHidden = false;
    }

    private float GetTilemapAlpha()
    {
        return tilemap.color.a;
    }

    private void SetTilemapAlpha(float alpha)
    {
        tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, alpha);
    }
}

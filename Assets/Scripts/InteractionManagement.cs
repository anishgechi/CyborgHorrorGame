using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionManagement : MonoBehaviour
{
    public float CheckingRate = 0.04f;
    public float MaxInteractRange;
    public LayerMask LayerMask;
    public TextMeshProUGUI PromptText;
    public float MaxAngleToInteract = 30f;
    private float TimeCheckedLast;
    private GameObject CurrentInteractGameObject;
    private IInteractable CurrentIntereacted;
    private Camera CamOBJ;

    // Start is called before the first frame update
    void Start()
    {
        CamOBJ = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (Time.time - TimeCheckedLast > CheckingRate)
        {
            TimeCheckedLast = Time.time;
            Ray raypointer = CamOBJ.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(raypointer, out hit, MaxInteractRange, LayerMask))
            {
                if (Vector3.Angle(CamOBJ.transform.forward, hit.point - CamOBJ.transform.position) <= MaxAngleToInteract)
                {
                    if (hit.collider.gameObject != CurrentInteractGameObject)
                    {
                        CurrentInteractGameObject = hit.collider.gameObject;
                        CurrentIntereacted = hit.collider.GetComponent<IInteractable>();
                        ShowPromptText();
                    }
                }
                else
                {
                    CurrentInteractGameObject = null;
                    CurrentIntereacted = null;
                    PromptText.gameObject.SetActive(false);
                }
            }
            else
            {
                CurrentInteractGameObject = null;
                CurrentIntereacted = null;
                PromptText.gameObject.SetActive(false);
            }
        }
    }

    void Interact()
    {
        if (CurrentIntereacted != null)
        {
            CurrentIntereacted.OnInteraction();
            HidePrompt();
        }
    }

    void HidePrompt()
    {
        PromptText.gameObject.SetActive(false);
        CurrentInteractGameObject = null;
        CurrentIntereacted = null;
    }

    void ShowPromptText()
    {
        PromptText.gameObject.SetActive(true);

        if (CurrentIntereacted != null)
        {
            PromptText.text = string.Format("<b>[E]<b>{0}", CurrentIntereacted.GetInteractionPrompt());
        }
        else
        {
            PromptText.text = "<b>[E]<b>Interact";
        }
    }

}

public interface IInteractable
{
    string GetInteractionPrompt();
    void OnInteraction();
}

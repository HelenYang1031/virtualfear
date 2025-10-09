using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A universal trigger script that handles all aspects of trigger detection,
/// object setup (Rigidbody, MeshRenderer), and custom actions via Unity Events.
/// </summary>
[RequireComponent(typeof(Collider))] // Ensure a Collider is present
public class TriggerManager : MonoBehaviour
{
    [Tooltip("If checked, the trigger will only fire the OnTriggerEnter event once, then disable the script.")]
    public bool isOneTimeTrigger = false;

    [Tooltip("The tag(s) of the object(s) that are allowed to activate this trigger. Leave empty to allow any object.")]
    public string[] allowedTags;

    [Header("--- Trigger Events ---")]
    [Tooltip("Actions to execute ONLY the moment a valid object ENTERS the trigger.")]
    public UnityEvent onTriggerEnter = new UnityEvent();

    [Tooltip("Actions to execute continuously while a valid object STAYS within the trigger.")]
    public UnityEvent onTriggerStay = new UnityEvent();

    [Tooltip("Actions to execute ONLY the moment a valid object EXITS the trigger.")]
    public UnityEvent onTriggerExit = new UnityEvent();

    // A flag to track if the one-time trigger has been activated.
    private bool hasBeenTriggered = false;

    private void Awake()
    {
        // 1. Rigidbody Setup (for reliable trigger events)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Ensure the Rigidbody is set up for a stationary, non-physical trigger.
        rb.useGravity = false;
        rb.isKinematic = true;

        // 2. Ensure the Collider is set to be a trigger.
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void Start()
    {
        // 3. MeshRenderer Hiding (for invisible runtime volume)
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for allowed tag AND if it's a one-time trigger that's already fired.
        if (IsAllowed(other) && !(isOneTimeTrigger && hasBeenTriggered))
        {
            onTriggerEnter.Invoke();

            // Handle one-time functionality
            if (isOneTimeTrigger)
            {
                hasBeenTriggered = true;
                // Optionally disable the entire script/component after use
                // enabled = false; 
            }
        }
        Debug.Log("enter");
    }

    private void OnTriggerStay(Collider other)
    {
        // Do not allow 'Stay' event to fire if it's a one-time trigger that's already fired.
        if (IsAllowed(other) && !(isOneTimeTrigger && hasBeenTriggered))
        {
            onTriggerStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Do not allow 'Exit' event to fire if it's a one-time trigger that's already fired.
        if (IsAllowed(other) && !(isOneTimeTrigger && hasBeenTriggered))
        {
            onTriggerExit.Invoke();
        }
    }

    /// <summary>
    /// Checks if the collider's GameObject has an allowed tag.
    /// </summary>
    private bool IsAllowed(Collider other)
    {
        if (allowedTags == null || allowedTags.Length == 0)
        {
            return true;
        }

        foreach (string tag in allowedTags)
        {
            if (other.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}
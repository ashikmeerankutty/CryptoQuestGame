using UnityEngine;

public class AmmoPattern : MonoBehaviour, IFireable
{
    #region Tooltip
    [Tooltip("Populate the array with the child ammo gameobjects")]
    #endregion
    [SerializeField] private Ammo[] ammoArray;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement)
    {
        this.ammoDetails = ammoDetails;

        this.ammoSpeed = ammoSpeed;

        // Set fire direction
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        // Set ammo range
        ammoRange = ammoDetails.ammoRange;

        // Activate ammo pattern gameobject
        gameObject.SetActive(true);

        // Loop through all child ammo and initialise it
        foreach (Ammo ammo in ammoArray)
        {
            ammo.InitialiseAmmo(ammoDetails, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, true);
        }

        // Set ammo charge timer - this will hold the ammo briefly
        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
        }
        else
        {
            ammoChargeTimer = 0f;
        }
    }

    private void Update()
    {
        // Ammo charge effect
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }

        // Calculate distance vector to move ammo
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        // Rotate ammo
        transform.Rotate(new Vector3(0f, 0f, ammoDetails.ammoRotationSpeed * Time.deltaTime));

        // Disable after max range reached
        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }

    }

    /// <summary>
    /// Set ammo fire direction based on the input angle and direction adjusted by the
    /// random spread
    /// </summary>
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        // calculate random spread angle between min and max
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        // Get a random spread toggle of 1 or -1
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }

        // Adjust ammo fire angle angle by random spread
        fireDirectionAngle += spreadToggle * randomSpread;

        // Set ammo fire direction
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    /// <summary>
    /// Disable the ammo - thus returning it to the object pool
    /// </summary>
    private void DisableAmmo()
    {
        // Disable the ammo pattern game object
        gameObject.SetActive(false);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoArray), ammoArray);
    }
#endif
    #endregion
}
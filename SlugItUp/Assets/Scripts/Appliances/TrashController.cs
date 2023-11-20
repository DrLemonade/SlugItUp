using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : ApplianceController
{

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + 0.5f);
    }

    public override bool insertSlug(Slug slug) {
        return true;
    }

    public override Slug getSlug() {
        return null;
    }

}

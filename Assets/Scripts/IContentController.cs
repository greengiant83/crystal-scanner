using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContentController
{
    event System.Action OnContentHidden;
    void HideContent();
}

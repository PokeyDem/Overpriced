using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasDisplayChoices
{
    public List<DisplayContext> PossibleDisplayChoices { get; set; }
}

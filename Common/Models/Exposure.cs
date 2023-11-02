using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models;

/// <summary>
/// The exposure of a paste determines the visibility to the users.
/// 
/// The author of the paste can and always will be able to see their own pastes.
/// 
/// **Unlisted** pastes are not shown in the author's profile.
/// 
/// **Public** pastes are always shown.
/// 
/// **Private** pastes are only accessible to the author.
/// 
/// This schema is an **INTEGER**. Not a string.
/// </summary>
public enum Exposure
{
    /// <summary>
    /// Unlisted. Paste will not be shown in the author's profile.
    /// </summary>
    Unlisted,

    /// <summary>
    /// Public. Paste will be shown.
    /// </summary>
    Public,

    /// <summary>
    /// Private. Paste is only visible to the author.
    /// </summary>
    Private,
}

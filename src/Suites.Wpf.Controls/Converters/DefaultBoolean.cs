using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Wpf.Converters
{
    /// <summary>
    ///     <para>Lists values which specify the validity of a condition (indicates whether the condition is true or false).
    /// </para>
    /// </summary>
    public enum DefaultBoolean
    {
        /// <summary>
        ///     <para>Corresponds to a Boolean value of <b>true</b>.
        /// </para>
        /// </summary>
        True,
        /// <summary>
        ///     <para>Corresponds to a Boolean value of <b>false</b>.
        /// </para>
        /// </summary>
        False,
        /// <summary>
        ///     <para>The value is determined by the current object's parent object setting (e.g., a control setting).
        /// </para>
        /// </summary>
        Default
    }
}

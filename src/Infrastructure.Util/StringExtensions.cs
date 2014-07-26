using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Infrastructure.Util
{
    /// <summary>
    /// Extension methods for <see cref="String"/>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the format item in a specified <see cref="T:System.String"/> with the text equivalent of the value of a specified <see cref="T:System.Object"/> instance.
        /// </summary>
        /// 
        /// <returns>
        /// A copy of <paramref name="format"/> in which the first format item has been replaced by the <see cref="T:System.String"/> equivalent of <paramref name="arg0"/>.
        /// </returns>
        /// <param name="format">A composite format string.</param><param name="arg0">An <see cref="T:System.Object"/> to format. </param><exception cref="T:System.ArgumentNullException"><paramref name="format"/> is null. </exception><exception cref="T:System.FormatException">The format item in <paramref name="format"/> is invalid.-or- The number indicating an argument to format is less than zero, or greater than or equal to the number of specified objects to format.</exception>
        [StringFormatMethod("format")]
        public static string FormatWith(this string format, object arg0)
        {
            format.ShouldNotBeNull("format");

            return String.Format(format, arg0);
        }

        /// <summary>
        /// Replaces the format item in a specified <see cref="T:System.String"/> with the text equivalent of the value of two specified <see cref="T:System.Object"/> instances.
        /// </summary>
        /// 
        /// <returns>
        /// A copy of <paramref name="format"/> in which the first and second format items have been replaced by the <see cref="T:System.String"/> equivalents of <paramref name="arg0"/> and <paramref name="arg1"/>.
        /// </returns>
        /// <param name="format">A composite format string. </param><param name="arg0">The first <see cref="T:System.Object"/> to format. </param><param name="arg1">The second <see cref="T:System.Object"/> to format. </param><exception cref="T:System.ArgumentNullException"><paramref name="format"/> is null. </exception><exception cref="T:System.FormatException"><paramref name="format"/> is invalid.-or- The number indicating an argument to format is less than zero, or greater than or equal to the number of specified objects to format. </exception><filterpriority>1</filterpriority>
        [StringFormatMethod("format")]
        public static string FormatWith(this string format, object arg0, object arg1)
        {
            format.ShouldNotBeNull("format");

            return String.Format(format, arg0, arg1);
        }

        /// <summary>
        /// Replaces the format item in a specified <see cref="T:System.String"/> with the text equivalent of the value of three specified <see cref="T:System.Object"/> instances.
        /// </summary>
        /// 
        /// <returns>
        /// A copy of <paramref name="format"/> in which the first, second, and third format items have been replaced by the <see cref="T:System.String"/> equivalents of <paramref name="arg0"/>, <paramref name="arg1"/>, and <paramref name="arg2"/>.
        /// </returns>
        /// <param name="format">A composite format string. </param><param name="arg0">The first <see cref="T:System.Object"/> to format. </param><param name="arg1">The second <see cref="T:System.Object"/> to format. </param><param name="arg2">The third <see cref="T:System.Object"/> to format. </param><exception cref="T:System.ArgumentNullException"><paramref name="format"/> is null. </exception><exception cref="T:System.FormatException"><paramref name="format"/> is invalid.-or- The number indicating an argument to format is less than zero, or greater than or equal to the number of specified objects to format. </exception><filterpriority>1</filterpriority>
        [StringFormatMethod("format")]
        public static string FormatWith(this string format, object arg0, object arg1, object arg2)
        {
            format.ShouldNotBeNull("format");

            return String.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces the format item in a specified <see cref="T:System.String"/> with the text equivalent of the value of a corresponding <see cref="T:System.Object"/> instance in a specified array.
        /// </summary>
        /// 
        /// <returns>
        /// A copy of <paramref name="format"/> in which the format items have been replaced by the <see cref="T:System.String"/> equivalent of the corresponding instances of <see cref="T:System.Object"/> in <paramref name="args"/>.
        /// </returns>
        /// <param name="format">A composite format string. </param><param name="args">An <see cref="T:System.Object"/> array containing zero or more objects to format. </param><exception cref="T:System.ArgumentNullException"><paramref name="format"/> or <paramref name="args"/> is null. </exception><exception cref="T:System.FormatException"><paramref name="format"/> is invalid.-or- The number indicating an argument to format is less than zero, or greater than or equal to the length of the <paramref name="args"/> array. </exception><filterpriority>1</filterpriority>
        [StringFormatMethod("format")]
        public static string FormatWith(this string format, params object[] args)
        {
            format.ShouldNotBeNull("format");
            args.ShouldNotBeNull("args");

            return String.Format(format, args);
        }

        /// <summary>
        /// Concatenates the members of a constructed <see cref="T:System.Collections.Generic.IEnumerable`1"/> collection of type <see cref="T:System.String"/>, using the specified separator between each member.
        /// </summary>
        /// 
        /// <returns>
        /// A string that consists of the members of <paramref name="parts"/> delimited by the <paramref name="separator"/> string. If <paramref name="parts"/> has no members, the method returns <see cref="F:System.String.Empty"/>.
        /// </returns>
        public static string JoinBy(this IEnumerable<string> parts, string separator)
        {
            return string.Join(separator, parts);
        }

        /// <summary>
        /// Concatenates the members of a constructed <see cref="T:System.Collections.Generic.IEnumerable`1"/> collection of type <see cref="T:System.String"/>.
        /// </summary>
        /// 
        /// <returns>
        /// A string that consists of the members of <paramref name="parts"/>. If <paramref name="parts"/> has no members, the method returns <see cref="F:System.String.Empty"/>.
        /// </returns>
        public static string Join(this IEnumerable<string> parts)
        {
            var sb = new StringBuilder();

            foreach (var part in parts)
            {
                sb.Append(part);
            }

            return sb.ToString();
        }
    }
}
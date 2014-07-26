using System;
using System.Collections;

namespace Infrastructure.Util
{
    /// <summary>
    /// Extension methods for contract checking.
    /// </summary>
    public static class ArgumentAssertionExtensions
    {
        /// <summary>
        /// Throw <see cref="ArgumentNullException"/> when <paramref name="argument"/> is null.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="argument">Argument to check.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>Argument value.</returns>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        public static T ShouldNotBeNull<T>(this T argument, string argumentName)
            where T : class
        {
            if (ReferenceEquals(argument, null))
            {
                throw new ArgumentNullException(argumentName);
            }

            return argument;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/> when <paramref name="argument"/> is less than or equal to zero.
        /// </summary>
        /// <param name="argument">Argument to check.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>Argument value.</returns>
        /// <exception cref="ArgumentNullException">Argument is less than or equal to zero.</exception>
        public static int ShouldBePositive(this int argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentException(argumentName + " should be greater than zero", argumentName);
            }

            return argument;
        }

        public static decimal ShouldNotBeNegative(this decimal argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentException(argumentName + " should be greater or equal to zero", argumentName);
            }

            return argument;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/> when <paramref name="collection"/> is empty.
        /// </summary>
        /// <typeparam name="T">Type of an element</typeparam>
        /// <param name="collection">Collection to check</param>
        /// <param name="argumentName">Name of the argument</param>
        /// <returns>Original collection</returns>
        /// <exception cref="ArgumentNullException">Collection is empty</exception>
        public static T ShouldNotBeEmpty<T>(this T collection, string argumentName)
            where T: ICollection
        {
            if (collection.Count == 0)
            {
                throw new ArgumentException(argumentName + " should not be empty", argumentName);
            }

            return collection;
        }
    }
}
using System;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Guards;
using GraphQLQueryBuilder.Implementations.Language;

namespace GraphQLQueryBuilder
{
    public static class ArgumentBuilder
    {
        /// <summary>
        /// Creates an <see cref="IArgument"/> from a string value.
        /// </summary>
        /// <param name="name">The argument's name'</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IArgument Build(string name, string value)
        {
            return Build(name, value, argValue => new StringArgumentValue(argValue));
        }

        /// <summary>
        /// Creates an <see cref="IArgument"/> from an integer value.
        /// </summary>
        /// <param name="name">The argument's name'</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IArgument Build(string name, int? value)
        {
            return Build(name, value, argValue => new IntegerArgumentValue(argValue.Value));
        }

        /// <summary>
        /// Creates an <see cref="IArgument"/> from a boolean value.
        /// </summary>
        /// <param name="name">The argument's name'</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IArgument Build(string name, bool? value)
        {
            return Build(name, value, argValue => new BooleanArgumentValue(argValue.Value));
        }

        /// <summary>
        /// Creates an <see cref="IArgument"/> from a floating point value.
        /// </summary>
        /// <param name="name">The argument's name'</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IArgument Build(string name, double? value)
        {
            return Build(name, value, argValue => new FloatArgumentValue(argValue.Value));
        }

        /// <summary>
        /// Creates an <see cref="IArgument"/> from an enum value.
        /// </summary>
        /// <param name="name">The argument's name'</param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IArgument Build<T>(string name, T value) where T : Enum
        {
            return Build(name, value, argValue => new EnumArgumentValue<T>(argValue));
        }

        /// <summary>
        /// Creates an <see cref="IArgument"/> that has an
        /// <see cref="INullArgumentValue"/>.
        /// </summary>
        /// <param name="name">The argument's name'</param>
        /// <returns></returns>
        public static IArgument Build(string name)
        {
            return Build<object>(name, null, _ => new NullArgumentValue());
        }

        private static IArgument Build<T>(string name, T value, Func<T, IArgumentValue> valueFactory)
        {
            name = name.MustNotBeNullOrWhiteSpace("A GraphQL argument name cannot be null or white space.", nameof(name))
                .MustBeValidGraphQLName(nameof(name));
            
            IArgumentValue argValue = value == null ? new NullArgumentValue() : valueFactory(value);

            return new Argument(name, argValue);
        }
    }
}
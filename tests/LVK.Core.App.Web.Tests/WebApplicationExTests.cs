using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Builder;

namespace LVK.Extensions.Bootstrapping.Web.Tests;

// public class WebApplicationExTests
// {
//     [Test]
//     public void ApiParity_WithWebApplication()
//     {
//         MethodInfo[] sourceMethods = typeof(WebApplication).GetMethods(BindingFlags.Static | BindingFlags.Public);
//         MethodInfo[] targetMethods = typeof(WebApplicationEx).GetMethods(BindingFlags.Static | BindingFlags.Public);
//
//         var unmatchedSourceMembers = (from sourceMethod in sourceMethods where !targetMethods.Any(targetMethod => IsMatch(sourceMethod, targetMethod)) select sourceMethod).ToList();
//         var unmatchedTargetMembers = (from targetMethod in targetMethods where !sourceMethods.Any(sourceMethod => IsMatch(sourceMethod, targetMethod)) select targetMethod).ToList();
//
//         Assert.That(unmatchedSourceMembers, Is.Empty);
//         Assert.That(unmatchedTargetMembers, Is.Empty);
//     }
//
//     private bool IsMatch(MethodInfo sourceMethod, MethodInfo targetMethod)
//     {
//         string sourceName = sourceMethod.Name.Replace("Builder", "");
//         if (sourceName != targetMethod.Name)
//             return false;
//
//         ParameterInfo[] sourceParameters = sourceMethod.GetParameters();
//         ParameterInfo[] targetParameters = targetMethod.GetParameters();
//         if (targetParameters.LastOrDefault() is { Name: "moduleBootstrapper" })
//             targetParameters = targetParameters[..^1];
//
//         if (!sourceParameters.Select(p => p.Name).SequenceEqual(targetParameters.Select(p => p.Name)))
//             return false;
//
//         if (!sourceParameters.Select(p => p.ParameterType).SequenceEqual(targetParameters.Select(p => p.ParameterType)))
//             return false;
//
//         return true;
//     }
// }
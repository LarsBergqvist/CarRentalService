This solution uses reflection to dynamically load implementation assemblies.
The assemblies are read from the executing assemblies path. Thus all dynamically 
loaded assemblies need to be placed in the same folder as the exe:s or test assemblies.

If using Resharper, turn off shadow-copying of assemblies in the unit test settings.

NUnit is used as test framework. To execute the NUnit tests with the test runner in Visual Studio 
without Resharper, install the NUnit test adapter extension.

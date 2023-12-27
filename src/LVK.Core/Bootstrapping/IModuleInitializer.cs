namespace LVK.Core.Bootstrapping;

public interface IModuleInitializer<in THost>
{
    void Initialize(THost host);
}
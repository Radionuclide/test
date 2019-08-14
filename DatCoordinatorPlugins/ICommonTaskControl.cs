using System;


namespace iba.Plugins
{
    public interface ICommonTaskControl
    {
        Guid ParentConfigurationGuid();
        int TaskIndex();
    }
}

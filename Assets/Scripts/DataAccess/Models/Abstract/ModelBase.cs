using System;

public abstract class ModelBase : IModelBase
{
    public ModelBase()
    {
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; set; }    
}
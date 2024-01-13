
namespace HTNDesigner.DataStructure.Variable
{
    public interface ISensorValue<T>
    {
        T GetValue();
        void SetValue(ISensorValue<T> value);
    }
}
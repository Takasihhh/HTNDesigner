using System;

namespace HTNDesigner.DataStructure.Variable
{
    public class SensorBool : SensorBase<bool>
    {
        
        public override Type m_SensorType
        {
            get
            {
                return typeof (bool);
            }
        }
        #region 对比函数
    public bool Equals(SensorBool other)
    {
        // If parameter is null, return false.
        if (Object.ReferenceEquals(other, null))
        {
            return false;
        }
        // Optimization for a common success case.
        if (Object.ReferenceEquals(this, other))
        {
            return true;
        }
        // If run-time types are not exactly the same, return false.
        if (this.GetType() != other.GetType())
        {
            return false;
        }
        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return this._value == other.GetValue();
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj.GetType() != this.GetType())
        {
            return false;
        }
        return Equals((SensorBool) obj);
    }
    public override int GetHashCode()
    {
        return this._value.GetHashCode();
    }
    public static bool operator ==(SensorBool lhs, SensorBool rhs)
    {
        // Check for null on left side.
        if (Object.ReferenceEquals(lhs, null))
        {
            if (Object.ReferenceEquals(rhs, null))
            {
                // null == null = true.
                return true;
            }
            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }
    public static bool operator !=(SensorBool lhs, SensorBool rhs)
    {
        return !(lhs == rhs);
    }
    public static bool operator >(SensorBool lhs, SensorBool rhs)
    {
        return false;
    }
    public static bool operator <(SensorBool lhs, SensorBool rhs)
    {
        return false;
    }
    public static bool operator >=(SensorBool lhs, SensorBool rhs)
    {
        return false;
    }
    public static bool operator <=(SensorBool lhs, SensorBool rhs)
    {
        return false;
    }
    #endregion
        
    }
    
}

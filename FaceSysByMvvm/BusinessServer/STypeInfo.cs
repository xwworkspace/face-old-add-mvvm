/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


/// <summary>
/// *���͹��� by fuyunpeng 20161212**
/// </summary>
#if !SILVERLIGHT
[Serializable]
#endif
public partial class STypeInfo : TBase
{
  private int _type;
  private string _description;

  public int Type
  {
    get
    {
      return _type;
    }
    set
    {
      __isset.type = true;
      this._type = value;
    }
  }

  public string Description
  {
    get
    {
      return _description;
    }
    set
    {
      __isset.description = true;
      this._description = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool type;
    public bool description;
  }

  public STypeInfo() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              Type = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Description = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("STypeInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Type);
        oprot.WriteFieldEnd();
      }
      if (Description != null && __isset.description) {
        field.Name = "description";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Description);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("STypeInfo(");
    bool __first = true;
    if (__isset.type) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Type: ");
      __sb.Append(Type);
    }
    if (Description != null && __isset.description) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Description: ");
      __sb.Append(Description);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}


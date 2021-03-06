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


#if !SILVERLIGHT
[Serializable]
#endif
public partial class ResourceCfg : TBase
{
  private string _rtype;
  private string _resid;
  private string _pid;

  public string Rtype
  {
    get
    {
      return _rtype;
    }
    set
    {
      __isset.rtype = true;
      this._rtype = value;
    }
  }

  public string Resid
  {
    get
    {
      return _resid;
    }
    set
    {
      __isset.resid = true;
      this._resid = value;
    }
  }

  public string Pid
  {
    get
    {
      return _pid;
    }
    set
    {
      __isset.pid = true;
      this._pid = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool rtype;
    public bool resid;
    public bool pid;
  }

  public ResourceCfg() {
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
            if (field.Type == TType.String) {
              Rtype = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Resid = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Pid = iprot.ReadString();
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
      TStruct struc = new TStruct("ResourceCfg");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Rtype != null && __isset.rtype) {
        field.Name = "rtype";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Rtype);
        oprot.WriteFieldEnd();
      }
      if (Resid != null && __isset.resid) {
        field.Name = "resid";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Resid);
        oprot.WriteFieldEnd();
      }
      if (Pid != null && __isset.pid) {
        field.Name = "pid";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Pid);
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
    StringBuilder __sb = new StringBuilder("ResourceCfg(");
    bool __first = true;
    if (Rtype != null && __isset.rtype) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Rtype: ");
      __sb.Append(Rtype);
    }
    if (Resid != null && __isset.resid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Resid: ");
      __sb.Append(Resid);
    }
    if (Pid != null && __isset.pid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Pid: ");
      __sb.Append(Pid);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}


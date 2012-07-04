# 10xLabs Windows hostnode service 

Windows service used to manage Windows based hostnodes/loopback VM.

Currently supports only EC2 loopback VMs, with following configuration passed to instance via `user-data`

    {
      "endpoint": "http://microcloud.endpoint.url",
      // TODO authorization key
    }

## Terminology

* **hostnode** acts as a resource within 10xLabs infrastructure, allocated within pools.
* **VM** - Virtual Machine acts as an abstraction of a single computing machine, both physical and virtualized. 
* **Loopback VM/Hostnode** - special type of VM which directly represents hostnode. Such a hostnode does not offer additional provisioning capabilities. Used in situation when access to underlying hypervisor is not available, or is facilitated using resource provider (like Amazon EC2). Also applicable to physical machines.


## TODO

* Microcloud authentication and authorization

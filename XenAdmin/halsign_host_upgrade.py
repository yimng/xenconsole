#!/usr/bin/env python

import logging
import os
import os.path
import shutil
import sys
import StringIO
import syslog
import XenAPIPlugin
# 1001 md5 check failure
# 1002 tar faile
# 1003 filename is empty
# 1004 toDirectory is empty
# 1005 Installation file 'install_patch'  No found
# 1006 Installation package  No found
# 
def main(session,args):
   syslog.syslog("patch ugrade.................")

   syslog.syslog("md5 check.................")
   
   try:
       filename=args["filename"]
       if filename == "":
          syslog.syslog("filename is empty")
          return "1003"
   except KeyError:
       raise RuntimeError("No argument found with key 'filename'.")
       
   try:
       toDirectory=args["toDirectory"]
       if filename == "":
          syslog.syslog("toDirectory is empty")
          return "1004"
   except KeyError:
       raise RuntimeError("No argument found with key 'toDirectory'.")
   
   
   if os.path.exists(toDirectory+'/'+filename+'.tgz'):
        syslog.syslog("Installation package found")
   else:
        syslog.syslog("Installation package  No found")
        return '1006'
        
   tmp = os.popen('md5sum '+toDirectory+'/'+filename+'.tgz;echo $?').readlines()   
   tmplen=len(tmp)
   tmpstatus=tmp[tmplen-1].strip('\n')
   if tmpstatus!='0':
      return "1001"

   md5v=tmp[0].split("  ")

   syslog.syslog(md5v[0])
   try:
       imd5v=args["md5"]
       if imd5v == md5v[0]:
          syslog.syslog("md5 is true")
       else :
          syslog.syslog("md5 is false")
          return "1001"
   except KeyError:
       raise RuntimeError("No argument found with key 'md5'.")

   syslog.syslog("tar.................")
    
   tmp=os.popen('tar -zxvf '+toDirectory+'/'+filename+'.tgz -C '+toDirectory+';echo $?').readlines()
   tmplen=len(tmp)
   tmpstatus=tmp[tmplen-1].strip('\n')
   if tmpstatus!='0':
       return "1002"
   
   syslog.syslog("install.................")
   if os.path.exists(toDirectory+'/'+filename+'/install_patch'):
   	  syslog.syslog("Installation file found")
   else:
      syslog.syslog("Installation file 'install_patch'  No found")
      return "1005"
      
   tmp=os.popen('cd '+toDirectory+'/'+filename+' ;./install_patch;echo $?').readlines()
   tmplen=len(tmp)
   tmpstatus=tmp[tmplen-1].strip('\n')
   if tmpstatus!='0':
       return tmpstatus
       
   os.popen('rm -rf '+toDirectory+'/'+filename)
   syslog.syslog("patch ugrade over.................")
   return "true"
   
def revert(session,args):
   syslog.syslog("patch revert.................")

   try:
       filename=args["filename"]
       if filename == "":
          syslog.syslog("filename is empty")
          return "1003"
   except KeyError:
       raise RuntimeError("No argument found with key 'filename'.")
       
   try:
       toDirectory=args["toDirectory"]
       if filename == "":
          syslog.syslog("toDirectory is empty")
          return "1004"
   except KeyError:
       raise RuntimeError("No argument found with key 'toDirectory'.")
   
   
   if os.path.exists(toDirectory+'/'+filename+'.tgz'):
        syslog.syslog("Installation package found")
   else:
        syslog.syslog("Installation package  No found")
        return '1006'

   syslog.syslog("tar.................")
    
   tmp=os.popen('tar -zxvf '+toDirectory+'/'+filename+'.tgz -C '+toDirectory+';echo $?').readlines()
   tmplen=len(tmp)
   tmpstatus=tmp[tmplen-1].strip('\n')
   if tmpstatus!='0':
       return "1002"
   
   syslog.syslog("uninstall.................")
   if os.path.exists(toDirectory+'/'+filename+'/uninstall_patch'):
      syslog.syslog("UnInstallation file found")
   else:
      syslog.syslog("UnInstallation file 'uninstall'  No found")
      return "1005"
      
   tmp=os.popen('cd '+toDirectory+'/'+filename+' ;./uninstall_patch;echo $?').readlines()
   tmplen=len(tmp)
   tmpstatus=tmp[tmplen-1].strip('\n')
   if tmpstatus!='0':
       return tmpstatus

   syslog.syslog("patch uninstall over.................")
   os.popen('rm -rf '+toDirectory+'/'+filename)
   return "true"

if __name__ == "__main__":
   XenAPIPlugin.dispatch({"main": main, "revert": revert})

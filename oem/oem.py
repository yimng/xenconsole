#!/bin/python

from os import path
from shutil import copyfile, copy
from io import open
import fnmatch
import os

resxfiles = "oem_files.conf"
corpconf = "oem.properties"
rcconf = "oem_resources.conf"
src = path.abspath("resources")
dst = path.abspath("..\..")

resx=[]
def getResx(dir, resx):
    for root, dirs, files in os.walk(dir):
        for f in fnmatch.filter(files, '*.resx'):
            resx.append(os.path.join(root, f))
getResx('..\..\XenAdmin', resx)
getResx('..\..\XenModel', resx)

# get oem name
old = []
new = []
cf = open(corpconf, encoding='utf-8')
for kv in cf:
    if kv.startswith('#'):
        continue
    if kv =="":
        continue
    (k, v) = kv.split('=')
    old.append(k)
    new.append(v.rstrip())
cf.close()

print("---------------------------------------------------------")
for o, n in zip(old, new):
    print(o + "=====>" + n)
print("---------------------------------------------------------")

for resx_file in resx:
    print(resx_file)
    try:
        lines = []
        with open(resx_file, encoding='utf-8') as infile:
            for line in infile:
                for o,n in zip(old, new):
                    line = line.replace(o, n)
                lines.append(line)
        with open(resx_file, 'w', encoding='utf-8') as outfile:
            for line in lines:
                outfile.write(line)
    except:  
        lines = []
        with open(resx_file) as infile:
            for line in infile:
                for o,n in zip(old, new):
                    line = line.replace(o, n)
                lines.append(line)
        with open(resx_file, 'w') as outfile:
            for line in lines:
                outfile.write(line)
# replace the resx files
ff = open(resxfiles)
encode = 'ascii'
for sf in ff:
    f = path.join(dst, path.normpath(sf.rstrip()))
    print(f)
    try:
        lines = []
        with open(f, encoding='utf-8') as infile:
            for line in infile:
                for o,n in zip(old, new):
                    line = line.replace(o, n)
                lines.append(line)
        with open(f, 'w', encoding='utf-8') as outfile:
            for line in lines:
                outfile.write(line)
    except:  
        lines = []
        with open(f) as infile:
            for line in infile:
                for o,n in zip(old, new):
                    line = line.replace(o, n)
                lines.append(line)
        with open(f, 'w') as outfile:
            for line in lines:
                outfile.write(line)
ff.close()

# replace the images
rf = open(rcconf)
for pic in rf:
    (k, v) = pic.split('=')
    copyfile(path.join(src, path.normpath(k)), path.join(dst, path.normpath(v.rstrip())))



#!/bin/python

from os import path
from shutil import copyfile
from io import open

resxfiles = "oem_files.conf"
corpconf = "oem.properties"
rcconf = "oem_resources.conf"
src = path.abspath("resources")
dst = path.abspath("..")

# get oem name
old = []
new = []
cf = open(corpconf, encoding='utf-8')
for line in cf:
    if line.startswith('#'):
        continue
    (k, v) = line.split('=')
    old.append(k)
    new.append(v.rstrip())
cf.close()

# replace the resx files
ff = open(resxfiles)
encode = 'ascii'
for line in ff:
    print(path.join(dst, path.normpath(line.rstrip())))
    try:
        f = open(path.join(dst, path.normpath(line.rstrip())), 'r')
        s = f.read()
        f.close()       
        f = open(path.join(dst, path.normpath(line.rstrip())), 'r+')
        for o,n in zip(old, new):
            s = s.replace(o, n)
        f.write(s)
    except:
        f = open(path.join(dst, path.normpath(line.rstrip())), 'r', encoding='utf-8')
        s = f.read()
        f.close()
        f = open(path.join(dst, path.normpath(line.rstrip())), 'r+', encoding='utf-8')
        for o,n in zip(old, new):
            s = s.replace(o, n)
        f.write(s)
    f.close()
ff.close()

# replace the images
rf = open(rcconf)
for line in rf:
    (k, v) = line.split('=')
    copyfile(path.join(src, path.normpath(k)), path.join(dst, path.normpath(v.rstrip())))

# replace in file brand
s=""
with open("../XenAdmin/Branding.cs","r", encoding='utf-8') as brand:    
    s = brand.read()
    for o,n in zip(old, new):
            s = s.replace(o, n)
with open("../XenAdmin/Branding.cs", "r+", encoding='utf-8') as newbrand:
    newbrand.write(s)


	
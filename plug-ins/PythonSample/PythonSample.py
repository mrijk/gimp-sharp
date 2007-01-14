import sys

import clr
clr.AddReference("gimp-sharp.dll")

from Gimp import Gimp
from Gimp import PythonPlugin
from Gimp import Procedure
from System import Array

class PythonSample(PythonPlugin):
    def __init__(self, args, package):
        self.name = package

    def ListProceduresTwo(self):
        print "ListProcedures"
        procedure = Procedure("plug_in_python_sample",
                              "Fixme!",
                              "Fixme!",
                              "Maurits Rijk",
                              "(C) Maurits Rijk",
                              "2004-2007",
                              "PythonSample...",
                              "RGB*, GRAY*")
        yield procedure

def Main(args):
    print "Inside Main!"
    plugin = PythonSample(Array[str](args), "PythonSample")

if __name__ == "__main__":
    if (len(sys.argv) > 1):
        Main(sys.argv)

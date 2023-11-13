import sys
import re
from collections import Counter
import subprocess
import os

os.chdir(sys.argv[1]) # path to aqusa-core project folder
input_folder = sys.argv[2] # sub folder in input folder of aqusa-core
total_count = 0
all_defects = []
skipped = []

for filename in os.listdir(os.path.join('input', input_folder)):
    input = os.path.join(input_folder, filename)
    output = filename.removesuffix('.txt') + '-result'
    result = subprocess.run(['python', 'aqusacore.py', '-i', input, '-o', output, '-f', 'txt'])
    try: 
        with open(os.path.join('output', output + '.txt'), 'r') as f:
            count = sum(1 for line in f if line.strip())
            print('Total count: ', count)
            total_count += count
            f.seek(0)
            defects = re.findall('Defect type: (.*)', f.read())
            print(Counter(defects)) 
            all_defects += defects
    except FileNotFoundError:
        skipped.append(filename)
    print()

print('SUMMARY')
print('Total count: ', total_count)
print(Counter(all_defects)) 
print('Skipped: ', skipped)

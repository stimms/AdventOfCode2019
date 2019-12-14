from collections import defaultdict;
import math
def read_input(filename):
  out_qtys = {}
  reactions = {}
  with open(filename) as f:
    for line in f:
      # Maybe .rstrip()
      ins, out = line.strip().split(' => ')
      oqty, out = out.strip().split(' ')
      ins = ins.split(', ')
      stuff = []
      for inp in ins:
        quant, name = inp.split(' ')
        stuff.append((int(quant), name))
      reactions[out] = stuff
      if out in out_qtys:
        raise Exception('multiple '+out)
      out_qtys[out] = int(oqty)
  return reactions, out_qtys

def undo_reactions(inv, reactions, reaction_output_qtys):
  produced_inventory = defaultdict(lambda:0, **inv)

  while True:
    eligible_mats = [mat for mat in produced_inventory.keys()
                      if produced_inventory[mat] > 0 and mat != 'ORE']
    if not eligible_mats:
      return produced_inventory['ORE']
    
    have_mat = eligible_mats[0]
    have_qty = produced_inventory[have_mat]
    
    formula_run_count = math.ceil(have_qty / reaction_output_qtys[have_mat])
    reaction_inputs = reactions[have_mat]

    for reaction_input in reaction_inputs:
      input_qty, input_mat = reaction_input
      produced_inventory[input_mat] += input_qty * formula_run_count    
    produced_inventory[have_mat] -= reaction_output_qtys[have_mat] * formula_run_count

def main():
  reactions, out_qtys = read_input('input.txt')

  part1 = undo_reactions({'FUEL':1}, reactions, out_qtys)
  print('Part 1: '+str(part1))

  def can_make(x):
    ores = undo_reactions( {'FUEL':x}, reactions, out_qtys)
    return ores <= 1000000000000
  
  

if __name__ == '__main__':
    main()

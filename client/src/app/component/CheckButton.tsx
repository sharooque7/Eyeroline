import { FormGroup, FormControlLabel, Checkbox } from "@mui/material";
import { useState } from "react";

interface Props {
  items: string[];
  checked?: string[];
  onChange: (items: string[]) => void;
}
const CheckButton = ({ items, checked, onChange }: Props) => {
  const [checkedItem, setCheckedItem] = useState(checked || []);

  function handleChecked(value: string) {
    const currentIndex = checkedItem.findIndex((item) => item === value);
    let newChecked: string[] = [];
    if (currentIndex === -1) newChecked = [...checkedItem, value];
    else newChecked = checkedItem.filter((item) => item !== value);
    setCheckedItem(newChecked);
    onChange(newChecked);
  }
  return (
    <FormGroup>
      {items.map((item) => (
        <FormControlLabel
          control={
            <Checkbox
              checked={checkedItem.indexOf(item) !== -1}
              onClick={() => handleChecked(item)}
            />
          }
          label={item}
          key={item}
        />
      ))}
    </FormGroup>
  );
};

export default CheckButton;

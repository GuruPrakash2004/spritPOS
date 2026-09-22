const ones = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
const tens = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

function indianNumberToWords(value: number): string {
  if (value < 20) return ones[value];
  if (value < 100) return tens[Math.floor(value / 10)] + (value % 10 ? ` ${ones[value % 10]}` : ' ');
  if (value < 1000) return `${ones[Math.floor(value / 100)]}Hundred ${value % 100 ? indianNumberToWords(value % 100) : ''}`;
  if (value < 100000) return `${indianNumberToWords(Math.floor(value / 1000))}Thousand ${value % 1000 ? indianNumberToWords(value % 1000) : ''}`;
  if (value < 10000000) return `${indianNumberToWords(Math.floor(value / 100000))}Lakh ${value % 100000 ? indianNumberToWords(value % 100000) : ''}`;
  return `${indianNumberToWords(Math.floor(value / 10000000))}Crore ${value % 10000000 ? indianNumberToWords(value % 10000000) : ''}`;
}

export function numberToWords(amount: number): string {
  if (!amount || Number.isNaN(amount)) return 'Zero Rupees Only';
  const whole = Math.floor(amount);
  const paisa = Math.round((amount - whole) * 100);
  const words = `Rupees ${indianNumberToWords(whole).trim()}`;
  return `${words}${paisa ? ` and ${indianNumberToWords(paisa).trim()} Paise` : ''} Only`;
}

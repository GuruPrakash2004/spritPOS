// Password rules for new or changed passwords, shown inline in Admin Master.
// Same rules as backend/app/passwords.py password_problem(); the server checks them again.
export const passwordProblem = (password: string, user: string): string | null => {
  if (password.length < 8) return 'Password must be at least 8 characters';
  if (password.trim().toLowerCase() === user.trim().toLowerCase()) return 'Password must not be the same as the user name';
  if (/^[a-z]+$/i.test(password) || /^\d+$/.test(password)) return 'Password must mix letters with numbers or symbols';
  return null;
};

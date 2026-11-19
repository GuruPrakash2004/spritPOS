// Checks the Dashboard's Softvent banner is one safe link to softvent.com.
import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';
import SoftventMarquee from './SoftventMarquee';

describe('SoftventMarquee', () => {
  it('links to softvent.com in a new tab', () => {
    render(<SoftventMarquee />);
    const link = screen.getByRole('link', { name: /softvent/i });
    expect(link).toHaveAttribute('href', 'https://softvent.com/');
    expect(link).toHaveAttribute('target', '_blank');
    expect(link.getAttribute('rel')).toContain('noopener');
  });
});

import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';
import { ScreenSkeleton, SkeletonTableRows } from './Skeleton';

describe('Skeleton', () => {
  it('table rows keep the table shape and say Loading once for screen readers', () => {
    render(<table><tbody><SkeletonTableRows rows={3} cols={4} /></tbody></table>);
    expect(screen.getAllByRole('row', { hidden: true })).toHaveLength(3);
    expect(screen.getAllByRole('cell', { hidden: true })).toHaveLength(12);
    expect(screen.getAllByText('Loading…')).toHaveLength(1);
  });

  it('a screen skeleton is a busy status region', () => {
    render(<ScreenSkeleton rows={3} />);
    expect(screen.getByRole('status', { busy: true })).toBeInTheDocument();
  });
});

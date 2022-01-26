import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import Header from './Header';

describe('<Header />', () => {
  test('it should mount', () => {
    render(<Header title="I have a title" />);
    
    const header = screen.getByTestId('Header');

    expect(header).toBeInTheDocument();
  });
});
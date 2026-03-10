import { render, screen, waitFor } from '@testing-library/react'
import { vi } from 'vitest'
import App from '../App'

const samplePosts = [
  {
    id: '1',
    title: 'Architecture Notes',
    slug: 'architecture-notes',
    status: 0,
    tags: ['architecture'],
    updatedAt: new Date().toISOString(),
    publishedAt: null,
  },
]

describe('App', () => {
  it('renders header and loads posts', async () => {
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: async () => samplePosts,
    })
    globalThis.fetch = fetchMock

    render(<App />)

    expect(screen.getByText('Personal Knowledge Publisher')).toBeInTheDocument()

    await waitFor(() => {
      expect(screen.getByText('Architecture Notes')).toBeInTheDocument()
    })

    expect(fetchMock).toHaveBeenCalledWith('/api/posts')
  })
})

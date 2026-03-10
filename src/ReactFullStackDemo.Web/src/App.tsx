import { useEffect, useMemo, useState } from 'react'
import './App.css'

type PostStatus = 0 | 1

type PostSummary = {
  id: string
  title: string
  slug: string
  status: PostStatus
  tags: string[]
  updatedAt: string
  publishedAt?: string | null
}

type PostDetail = {
  id: string
  title: string
  slug: string
  markdown: string
  status: PostStatus
  tags: string[]
  createdAt: string
  updatedAt: string
  publishedAt?: string | null
}

type CreatePostRequest = {
  title: string
  markdown: string
  status?: PostStatus
  tags?: string[]
}

type UpdatePostRequest = {
  title: string
  markdown: string
  status: PostStatus
  tags?: string[]
}

const statusLabel = (value: PostStatus) => (value === 1 ? 'Published' : 'Draft')

const apiBase = import.meta.env.VITE_API_BASE_URL?.trim() || '/api'

const normalizeBase = (base: string) => (base.endsWith('/') ? base.slice(0, -1) : base)

const buildUrl = (path: string) => `${normalizeBase(apiBase)}${path}`

async function apiGet<T>(path: string): Promise<T> {
  const response = await fetch(buildUrl(path))
  if (!response.ok) {
    throw new Error(`Request failed (${response.status})`)
  }
  return (await response.json()) as T
}

async function apiPost<T>(path: string, body: unknown): Promise<T> {
  const response = await fetch(buildUrl(path), {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })
  if (!response.ok) {
    throw new Error(`Request failed (${response.status})`)
  }
  return (await response.json()) as T
}

async function apiPut<T>(path: string, body: unknown): Promise<T> {
  const response = await fetch(buildUrl(path), {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })
  if (!response.ok) {
    throw new Error(`Request failed (${response.status})`)
  }
  return (await response.json()) as T
}

export default function App() {
  const [posts, setPosts] = useState<PostSummary[]>([])
  const [selectedId, setSelectedId] = useState<string | null>(null)
  const [title, setTitle] = useState('')
  const [markdown, setMarkdown] = useState('')
  const [status, setStatus] = useState<PostStatus>(0)
  const [tagsInput, setTagsInput] = useState('')
  const [loading, setLoading] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const tags = useMemo(
    () =>
      tagsInput
        .split(',')
        .map((tag) => tag.trim())
        .filter((tag) => tag.length > 0),
    [tagsInput],
  )

  const selectedPost = useMemo(
    () => posts.find((post) => post.id === selectedId) ?? null,
    [posts, selectedId],
  )

  useEffect(() => {
    void loadPosts()
  }, [])

  useEffect(() => {
    if (!selectedId) {
      return
    }

    let isCurrent = true
    setError(null)

    apiGet<PostDetail>(`/posts/${selectedId}`)
      .then((post) => {
        if (!isCurrent) return
        setTitle(post.title)
        setMarkdown(post.markdown)
        setStatus(post.status)
        setTagsInput(post.tags.join(', '))
      })
      .catch((err) => {
        if (!isCurrent) return
        setError(err instanceof Error ? err.message : 'Failed to load post')
      })

    return () => {
      isCurrent = false
    }
  }, [selectedId])

  const resetForm = () => {
    setSelectedId(null)
    setTitle('')
    setMarkdown('')
    setStatus(0)
    setTagsInput('')
    setError(null)
  }

  const loadPosts = async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await apiGet<PostSummary[]>('/posts')
      setPosts(data)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load posts')
    } finally {
      setLoading(false)
    }
  }

  const handleSave = async () => {
    setSaving(true)
    setError(null)
    try {
      if (!title.trim() || !markdown.trim()) {
        setError('Title and Markdown are required.')
        return
      }

      if (!selectedId) {
        const payload: CreatePostRequest = {
          title,
          markdown,
          status,
          tags,
        }
        const created = await apiPost<PostDetail>('/posts', payload)
        setSelectedId(created.id)
      } else {
        const payload: UpdatePostRequest = {
          title,
          markdown,
          status,
          tags,
        }
        await apiPut<PostDetail>(`/posts/${selectedId}`, payload)
      }

      await loadPosts()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Save failed')
    } finally {
      setSaving(false)
    }
  }

  const lastUpdated = selectedPost?.updatedAt
    ? new Date(selectedPost.updatedAt).toLocaleString()
    : null

  return (
    <div className="app">
      <header className="topbar">
        <div>
          <p className="eyebrow">ReactFullStackDemo</p>
          <h1>Personal Knowledge Publisher</h1>
          <p className="subtitle">
            Draft, edit, and publish markdown knowledge cards with a clean API.
          </p>
        </div>
        <button className="ghost" onClick={resetForm}>
          New Draft
        </button>
      </header>

      <div className="content">
        <aside className="panel list-panel">
          <div className="panel-header">
            <h2>Recent Posts</h2>
            <button className="small" onClick={loadPosts} disabled={loading}>
              {loading ? 'Loading...' : 'Refresh'}
            </button>
          </div>

          {posts.length === 0 && !loading ? (
            <p className="empty">No posts yet. Create the first draft.</p>
          ) : (
            <ul className="post-list">
              {posts.map((post) => (
                <li
                  key={post.id}
                  className={post.id === selectedId ? 'active' : ''}
                  onClick={() => setSelectedId(post.id)}
                >
                  <div>
                    <h3>{post.title}</h3>
                    <p>{post.slug}</p>
                  </div>
                  <span className={`status ${post.status === 1 ? 'live' : 'draft'}`}>
                    {statusLabel(post.status)}
                  </span>
                </li>
              ))}
            </ul>
          )}
        </aside>

        <section className="panel editor-panel">
          <div className="panel-header">
            <h2>{selectedId ? 'Edit Post' : 'Create Post'}</h2>
            <div className="meta">
              {lastUpdated && <span>Updated {lastUpdated}</span>}
              <span className="divider" />
              <span>{selectedId ? 'Editing' : 'Drafting'}</span>
            </div>
          </div>

          {error && <div className="error">{error}</div>}

          <div className="form-grid">
            <label>
              Title
              <input
                type="text"
                value={title}
                onChange={(event) => setTitle(event.target.value)}
                placeholder="Explain the idea in one line"
              />
            </label>

            <label>
              Status
              <select value={status} onChange={(event) => setStatus(Number(event.target.value) as PostStatus)}>
                <option value={0}>Draft</option>
                <option value={1}>Published</option>
              </select>
            </label>

            <label className="span-2">
              Tags (comma separated)
              <input
                type="text"
                value={tagsInput}
                onChange={(event) => setTagsInput(event.target.value)}
                placeholder="architecture, patterns, database"
              />
            </label>

            <label className="span-2">
              Markdown Content
              <textarea
                value={markdown}
                onChange={(event) => setMarkdown(event.target.value)}
                placeholder="Write the content in Markdown..."
              />
            </label>
          </div>

          <div className="actions">
            <button className="primary" onClick={handleSave} disabled={saving}>
              {saving ? 'Saving...' : 'Save Post'}
            </button>
            <div className="helper">
              <span>Status: {statusLabel(status)}</span>
              <span>API: {apiBase}</span>
            </div>
          </div>

          <div className="preview">
            <h3>Markdown Preview</h3>
            <div className="preview-card">
              <h4>{title || 'Untitled draft'}</h4>
              <p className="preview-meta">
                {tags.length > 0 ? tags.join(' · ') : 'No tags yet'}
              </p>
              <pre>{markdown || 'Write something to see the preview.'}</pre>
            </div>
          </div>
        </section>
      </div>
    </div>
  )
}

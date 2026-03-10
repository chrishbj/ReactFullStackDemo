import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize, take } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiBase = '/api';

  posts: PostSummary[] = [];
  selectedId: string | null = null;
  title = '';
  markdown = '';
  status: PostStatus = 0;
  tagsInput = '';
  loading = false;
  saving = false;
  error: string | null = null;

  get tags(): string[] {
    return this.tagsInput
      .split(',')
      .map((tag) => tag.trim())
      .filter((tag) => tag.length > 0);
  }

  get selectedPost(): PostSummary | null {
    return this.posts.find((post) => post.id === this.selectedId) ?? null;
  }

  async ngOnInit(): Promise<void> {
    await this.loadPosts();
  }

  statusLabel(value: PostStatus): string {
    return value === 1 ? 'Published' : 'Draft';
  }

  async loadPosts(): Promise<void> {
    this.loading = true;
    this.error = null;
    this.http
      .get<PostSummary[]>(`${this.apiBase}/posts`)
      .pipe(
        take(1),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe({
        next: (data) => {
          this.posts = data;
        },
        error: (err) => {
          this.error =
            err instanceof Error ? err.message : 'Failed to load posts';
        }
      });
  }

  async selectPost(id: string): Promise<void> {
    this.selectedId = id;
    this.error = null;
    this.http
      .get<PostDetail>(`${this.apiBase}/posts/${id}`)
      .pipe(take(1))
      .subscribe({
        next: (post) => {
          this.title = post.title;
          this.markdown = post.markdown;
          this.status = post.status;
          this.tagsInput = post.tags.join(', ');
        },
        error: (err) => {
          this.error =
            err instanceof Error ? err.message : 'Failed to load post';
        }
      });
  }

  newDraft(): void {
    this.selectedId = null;
    this.title = '';
    this.markdown = '';
    this.status = 0;
    this.tagsInput = '';
    this.error = null;
  }

  async save(): Promise<void> {
    this.saving = true;
    this.error = null;
    try {
      if (!this.title.trim() || !this.markdown.trim()) {
        this.error = 'Title and Markdown are required.';
        return;
      }

      if (!this.selectedId) {
        const payload: CreatePostRequest = {
          title: this.title,
          markdown: this.markdown,
          status: this.status,
          tags: this.tags
        };
        this.http
          .post<PostDetail>(`${this.apiBase}/posts`, payload)
          .pipe(
            take(1),
            finalize(() => {
              this.saving = false;
            })
          )
          .subscribe({
            next: (created) => {
              this.selectedId = created.id;
              this.loadPosts();
            },
            error: (err) => {
              this.error =
                err instanceof Error ? err.message : 'Save failed';
            }
          });
        return;
      }

      const payload: UpdatePostRequest = {
        title: this.title,
        markdown: this.markdown,
        status: this.status,
        tags: this.tags
      };
      this.http
        .put<PostDetail>(`${this.apiBase}/posts/${this.selectedId}`, payload)
        .pipe(
          take(1),
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe({
          next: () => {
            this.loadPosts();
          },
          error: (err) => {
            this.error = err instanceof Error ? err.message : 'Save failed';
          }
        });
      return;
    } catch (err) {
      this.error = err instanceof Error ? err.message : 'Save failed';
    } finally {
      this.saving = false;
    }
  }
}

type PostStatus = 0 | 1;

type PostSummary = {
  id: string;
  title: string;
  slug: string;
  status: PostStatus;
  tags: string[];
  updatedAt: string;
  publishedAt?: string | null;
};

type PostDetail = {
  id: string;
  title: string;
  slug: string;
  markdown: string;
  status: PostStatus;
  tags: string[];
  createdAt: string;
  updatedAt: string;
  publishedAt?: string | null;
};

type CreatePostRequest = {
  title: string;
  markdown: string;
  status?: PostStatus;
  tags?: string[];
};

type UpdatePostRequest = {
  title: string;
  markdown: string;
  status: PostStatus;
  tags?: string[];
};

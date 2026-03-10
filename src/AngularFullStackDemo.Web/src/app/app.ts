import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);
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
    try {
      const data = await firstValueFrom(
        this.http.get<PostSummary[]>(`${this.apiBase}/posts`)
      );
      this.posts = data;
    } catch (err) {
      this.error = err instanceof Error ? err.message : 'Failed to load posts';
    } finally {
      this.loading = false;
      this.cdr.detectChanges();
    }
  }

  async selectPost(id: string): Promise<void> {
    this.selectedId = id;
    this.error = null;
    try {
      const post = await firstValueFrom(
        this.http.get<PostDetail>(`${this.apiBase}/posts/${id}`)
      );
      this.title = post.title;
      this.markdown = post.markdown;
      this.status = post.status;
      this.tagsInput = post.tags.join(', ');
    } catch (err) {
      this.error = err instanceof Error ? err.message : 'Failed to load post';
    } finally {
      this.cdr.detectChanges();
    }
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
        const created = await firstValueFrom(
          this.http.post<PostDetail>(`${this.apiBase}/posts`, payload)
        );
        this.selectedId = created.id;
      } else {
        const payload: UpdatePostRequest = {
          title: this.title,
          markdown: this.markdown,
          status: this.status,
          tags: this.tags
        };
        await firstValueFrom(
          this.http.put<PostDetail>(
            `${this.apiBase}/posts/${this.selectedId}`,
            payload
          )
        );
      }

      await this.loadPosts();
    } catch (err) {
      this.error = err instanceof Error ? err.message : 'Save failed';
    } finally {
      this.saving = false;
      this.cdr.detectChanges();
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

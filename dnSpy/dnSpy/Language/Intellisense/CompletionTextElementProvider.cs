/*
    Copyright (C) 2014-2019 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Windows;
using dnSpy.Contracts.Language.Intellisense;
using dnSpy.Contracts.Language.Intellisense.Classification;
using dnSpy.Contracts.Text;
using dnSpy.Contracts.Text.Classification;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace dnSpy.Language.Intellisense {
	sealed class CompletionTextElementProvider : ICompletionTextElementProvider {
		readonly IClassificationFormatMap classificationFormatMap;
		readonly IContentTypeRegistryService contentTypeRegistryService;
		readonly ITextElementProvider textElementProvider;

		public CompletionTextElementProvider(IClassificationFormatMap classificationFormatMap, IContentTypeRegistryService contentTypeRegistryService, ITextElementProvider textElementProvider) {
			this.classificationFormatMap = classificationFormatMap ?? throw new ArgumentNullException(nameof(classificationFormatMap));
			this.contentTypeRegistryService = contentTypeRegistryService ?? throw new ArgumentNullException(nameof(contentTypeRegistryService));
			this.textElementProvider = textElementProvider ?? throw new ArgumentNullException(nameof(textElementProvider));
		}

		public FrameworkElement Create(CompletionSet completionSet, Completion completion, CompletionClassifierKind kind, bool colorize) {
			if (completionSet is null)
				throw new ArgumentNullException(nameof(completionSet));
			if (completion is null)
				throw new ArgumentNullException(nameof(completion));
			Debug.Assert(completionSet.Completions.Contains(completion));

			CompletionClassifierContext context;
			string defaultContentType;
			switch (kind) {
			case CompletionClassifierKind.DisplayText:
				var inputText = completionSet.ApplicableTo.GetText(completionSet.ApplicableTo.TextBuffer.CurrentSnapshot);
				context = new CompletionDisplayTextClassifierContext(completionSet, completion, completion.DisplayText, inputText, colorize);
				defaultContentType = ContentTypes.CompletionDisplayText;
				break;

			case CompletionClassifierKind.Suffix:
				var suffix = (completion as DsCompletion)?.Suffix ?? string.Empty;
				context = new CompletionSuffixClassifierContext(completionSet, completion, suffix, colorize);
				defaultContentType = ContentTypes.CompletionSuffix;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(kind));
			}

			var contentType = (completionSet as ICompletionSetContentTypeProvider)?.GetContentType(contentTypeRegistryService, kind);
			if (contentType is null)
				return textElementProvider.CreateTextElement(classificationFormatMap, context, defaultContentType, TextElementFlags.None);
			return textElementProvider.CreateTextElement(classificationFormatMap, context, contentType, TextElementFlags.None);
		}

		public void Dispose() { }
	}
}
